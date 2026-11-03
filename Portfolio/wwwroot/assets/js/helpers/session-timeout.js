export function createSessionTimeoutManager(config) {
  const {
    statusUrl,
    keepAliveUrl,
    pollIntervalSeconds = 30,
    warningSeconds = 300
  } = config;

  let warningShown = false;
  let lastKnownRemaining = null;
  let activeTimeoutId = null;

  const listeners = {
    warning: [],
    logout: [],
    tick: []
  };

  function on(event, handler) {
    if (listeners[event]) listeners[event].push(handler);
  }

  function emit(event, payload) {
    (listeners[event] || []).forEach(fn => fn(payload));
  }

  async function checkStatus() {
    let response;
    try {
      response = await fetch(statusUrl, {
        credentials: "include",
        headers: {"Accept": "application/json"}
      });
    } catch {
      // If error, just retry in default interval
      scheduleNextPoll(pollIntervalSeconds);
      return;
    }

    if (!response.ok) {
      emit("logout");
      return;
    }

    if (response.status === 302) {
      emit("logout");
      return;
    }

    const data = await response.json();
    const remaining = data.remainingSeconds;
    lastKnownRemaining = remaining;
    emit("tick", remaining);

    if (remaining <= 0) {
      emit("logout");
      return;
    }

    if (remaining <= warningSeconds) {
      if (!warningShown) {
        warningShown = true;
        emit("warning", remaining);
      }
      scheduleNextPoll(pollIntervalSeconds);
    } else {
      warningShown = false;

      const secondsUntilWarning = remaining - warningSeconds;
      let nextPollInSeconds = pollIntervalSeconds;

      if (secondsUntilWarning < pollIntervalSeconds) {
        nextPollInSeconds = secondsUntilWarning;
      }

      scheduleNextPoll(nextPollInSeconds);
    }
  }

  function scheduleNextPoll(seconds) {
    if (activeTimeoutId) clearTimeout(activeTimeoutId);

    const safeSeconds = Math.max(1, seconds);

    activeTimeoutId = setTimeout(checkStatus, safeSeconds * 1000);
  }

  async function keepAlive() {
    if (!keepAliveUrl) throw new Error("keepAliveUrl not configured");

    await fetch(keepAliveUrl, {method: "POST", credentials: "include"});
    warningShown = false;

    if (activeTimeoutId) clearTimeout(activeTimeoutId);
    await checkStatus();
  }

  async function start() {
    await checkStatus();
  }

  return {
    start,
    keepAlive,
    on,
    getRemainingSeconds: () => lastKnownRemaining
  };
}
