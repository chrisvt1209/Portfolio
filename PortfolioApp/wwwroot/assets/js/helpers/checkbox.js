/**
 * Retrieves the values of all checked form elements matching a selector.
 *
 * This function queries the DOM for all elements matching the provided
 * selector, filters to only those that are checked (e.g. checkboxes, radio buttons),
 * and returns an array of their `value` attributes.
 *
 * @param {string} selector - A CSS selector string used to match form elements.
 * @returns {string[]} An array of values from the checked elements.
 *
 * @example
 * // Given checkboxes:
 * // <input type="checkbox" name="fruit" value="apple" checked>
 * // <input type="checkbox" name="fruit" value="banana">
 * // <input type="checkbox" name="fruit" value="cherry" checked>
 *
 * getSelectedValues('input[name="fruit"]');
 * // returns ["apple", "cherry"]
 */

export const getSelectedValues = (selector) => {
    return Array.from(document.querySelectorAll(selector))
        .filter(element => element.checked)
        .map(element => element.value);
}

