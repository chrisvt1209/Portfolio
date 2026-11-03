import { createSessionTimeoutManager } from "./helpers/session-timeout.js";
import { addEventListener } from "./helpers/dom.js";

async function initSessionTimeoutManager() {
    const res = await fetch("/api/session/config", { credentials: "include" });
    const config = await res.json();

    const session = createSessionTimeoutManager({
        statusUrl: "/api/session/status",
        keepAliveUrl: "/api/session/keep-alive",
        pollIntervalSeconds: config.pollIntervalSeconds,
        warningSeconds: config.warningMinutes * 60
    });

    session.on("warning", async () => {
        await document.getElementById("session-expire-modal").show();
    });

    session.on("logout", () => {
        window.location.href = `/umbraco/surface/AuthSurface/Logout`;
    });

    addEventListener(document, "clicked", async () => {
        await session.keepAlive();
        await document.getElementById("session-expire-modal").hide();

    }, "#session-expire-modal-keep-alive")

    await session.start();
}

const main = async () => {
    if (window.location.href === HOME_URL) {
        await initSessionTimeoutManager();
    }
};

document.addEventListener("DOMContentLoaded", main);