/**
 * Adds an event listener to an element, optionally using event delegation.
 *
 * If a `selector` is provided, the event handler is only called when the
 * event originates from an element matching the selector or its descendants.
 * Otherwise, the handler is called directly on the element.
 *
 * The returned function is the wrapped event handler that was actually attached,
 * which can be used later to remove the listener with `removeEventListener`.
 *
 * @param {HTMLElement} el - The target element to attach the event listener to.
 * @param {string} eventName - The name of the event (e.g., "click", "input").
 * @param {Function} eventHandler - The function to call when the event occurs.
 * @param {string} [selector] - Optional CSS selector for event delegation.
 * @returns {Function} The wrapped event handler that was attached.
 *
 * @example
 * // Direct event listener
 * const handler = addEventListener(button, "click", (e) => console.log("Clicked!"));
 *
 * // Delegated event listener
 * const handler = addEventListener(list, "click", (e) => console.log(e.textContent), "li");
 *
 * // Removing the event listener later
 * button.removeEventListener("click", handler);
 */
export const addEventListener = (el, eventName, eventHandler, selector) => {
    if (selector) {
        const wrappedHandler = (e) => {
            if (!e.target) return;
            const el = e.target.closest(selector);
            if (el) {
                eventHandler.call(el, e);
            }
        };
        el.addEventListener(eventName, wrappedHandler, true);
        return wrappedHandler;
    } else {
        const wrappedHandler = (e) => {
            eventHandler.call(el, e);
        };
        el.addEventListener(eventName, wrappedHandler);
        return wrappedHandler;
    }
};
