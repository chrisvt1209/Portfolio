import { tryParseDate } from "./date.js";
import { format } from "date-fns";

/**
 * Sets the value of a form field inside a given element.
 *
 * Supports standard input fields, as well as custom elements like
 * `TMB-SELECT` and `TMB-DATEPICKER`.
 *
 * @param {HTMLElement} element - The parent element containing the form field.
 * @param {string} name - The `name` attribute of the form field to set.
 * @param {*} value - The value to set. Can be a string, number, or array for custom elements.
 *
 * @example
 * // Standard input
 * setFormFieldValue(formElement, "username", "alice");
 *
 * // TMB-SELECT
 * setFormFieldValue(formElement, "country", "US");
 */
export const setFormFieldValue = (element, name, value) => {
    const input = element.querySelector(`[name="${name}"]`);

    if (input != null) {
        if (input.tagName === "TMB-SELECT") {
            if (!!value) {
                input.selected = [value];
            } else {
                input.selected = [];
            }
        } else if (input.tagName === "TMB-DATEPICKER") {
            input.value = [value];
        } else {
            input.value = value;
        }
    }
};

/**
 * Sets multiple form field values based on an object mapping.
 *
 * Iterates through the object's entries and sets each corresponding
 * form field inside the given element. Dates are detected and formatted
 * as `yyyy-MM-dd` if they are valid.
 *
 * @param {HTMLElement} element - The parent element containing form fields.
 * @param {Object.<string, any>} object - An object mapping field names to values.
 *
 * @example
 * setFormFieldValuesForObject(formElement, {
 *   username: "alice",
 *   birthdate: "1990-01-01",
 *   country: "US"
 * });
 */
export const setFormFieldValuesForObject = (element, object) => {
    for (const entries of Object.entries(object)) {
        const tryParseResult = tryParseDate(entries[1]);
        const value = !!tryParseResult && isNaN(entries[1]) ? format(tryParseResult, "yyyy-MM-dd") : entries[1];

        setFormFieldValue(element, entries[0], value);
    }
}
