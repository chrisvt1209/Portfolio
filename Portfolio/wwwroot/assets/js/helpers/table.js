/**
 * Wraps content in a <span> that preserves whitespace and prevents line breaks.
 *
 * @param {string} content - The text content to wrap.
 * @returns {string} HTML string with the content inside a <span> styled with `white-space: pre`.
 *
 * @example
 * const html = singleLineColumn("Hello\nWorld");
 * // '<span style="white-space: pre">Hello\nWorld</span>'
 */
export const singleLineColumn = (content) => {
    return `<span style="white-space: pre">${content}</span>`;
};

/**
 * Returns an HTML string for a disabled checkbox representing a boolean value.
 *
 * The checkbox uses a custom `<tmb-checkbox>` element and is rendered as a switch.
 *
 * @param {boolean} isChecked - Whether the checkbox should be checked.
 * @returns {string} HTML string for the checkbox.
 *
 * @example
 * const html = formatBoolean(true);
 * // '<tmb-checkbox is-checked="true" is-switch is-disabled></tmb-checkbox>'
 */
export const formatBoolean = (isChecked) => {
    return `<tmb-checkbox is-checked="${isChecked}" is-switch is-disabled></tmb-checkbox>`;
};
