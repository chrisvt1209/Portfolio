/**
 * Retrieves a <tmb-option> element by its value from a custom select element.
 *
 * @param {HTMLElement} select - The custom select element.
 * @param {string} value - The value of the option to retrieve.
 * @returns {HTMLElement|null} The matching <tmb-option> element, or null if not found.
 *
 * @example
 * const option = getOptionElement(mySelect, "USD");
 */
export const getOptionElement = (select, value) => {
    return select.querySelector(`tmb-option[value="${value}"]`);
};

/**
 * Gets the name (text content) of the first selected option in a custom select element.
 *
 * @param {HTMLElement} select - The custom select element.
 * @returns {string} The text content of the first selected option, or an empty string if none selected.
 *
 * @example
 * const name = getSelectedOptionName(mySelect);
 */
export const getSelectedOptionName = (select) => {
    return getSelectedOptionNames(select)[0];
};

/**
 * Gets the names (text content) of all selected options in a custom select element.
 *
 * @param {HTMLElement} select - The custom select element.
 * @returns {string[]} An array of the text content of all selected options.
 *
 * @example
 * const names = getSelectedOptionNames(mySelect);
 */
export const getSelectedOptionNames = (select) => {
    return select.value.map(value => getOptionElement(select, value)?.textContent ?? "");
};

/**
 * Retrieves the <tmb-option> element corresponding to the first selected value.
 *
 * @param {HTMLElement} select - The custom select element.
 * @returns {HTMLElement|null} The selected <tmb-option> element, or null if none selected.
 *
 * @example
 * const selectedOption = getSelectedOptionElement(mySelect);
 */
export const getSelectedOptionElement = (select) => {
    return select.querySelector(`tmb-option[value="${select.selected[0]}"]`);
};

/**
 * Removes all <tmb-option> elements from a custom select element.
 *
 * @param {HTMLElement} select - The custom select element.
 *
 * @example
 * clearOptions(mySelect);
 */
export const clearOptions = (select) => {
    select.querySelectorAll("tmb-option").forEach(e => e.parentNode.removeChild(e));
};
