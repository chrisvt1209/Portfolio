import { get } from "./http.js";
import { format } from "date-fns";

/**
 * Fetches and caches currency icons from the server.
 *
 * The function is immediately invoked and returns an async function that,
 * on first call, fetches icon data from `/api/CurrencyApi/GetCurrencyIcons`
 * and caches it for subsequent calls.
 *
 * @function getCurrencyIcons
 * @async
 * @returns {Promise<Object.<string, string>>} A mapping of currency codes to icon names.
 *
 * @example
 * const icons = await getCurrencyIcons();
 * console.log(icons["USD"]); // e.g. "currency-dollar"
 */
export const getCurrencyIcons = function () {
    let data;
    return async () => {
        if (data)
            return data;
        const response = await get("/api/CurrencyApi/GetCurrencyIcons");
        data = response.json();
        return data;
    };
}();

/**
 * Updates the icon(s) inside a DOM element to match the given currency code.
 *
 * It replaces any existing currency-related classes with the appropriate one
 * from the currency icons set. Elements that should update must have the
 * `.currency-replace-target` class.
 *
 * @async
 * @param {HTMLElement} element - The parent DOM element containing targets to update.
 * @param {string} currencyCode - The ISO 4217 currency code (e.g. `"USD"`, `"EUR"`).
 * @returns {Promise<void>}
 *
 * @example
 * await setCurrencyIcons(document.getElementById("price"), "USD");
 */
export const setCurrencyIcons = async (element, currencyCode) => {
    const currencyIcons = await getCurrencyIcons();
    const currencyIcon = currencyIcons[currencyCode] ?? "currency-euro";
    const currencyIconClassName = `bi-${currencyIcon}`;
    const otherCurrencyIconClassNames = Object.keys(currencyIcons).map(icon => `bi-${icon}`);

    const currencyReplaceTargets = element.querySelectorAll(".currency-replace-target");
    currencyReplaceTargets.forEach(target => {
        target.classList.remove(...otherCurrencyIconClassNames);
        target.classList.add(currencyIconClassName);
    });
};

/**
 * Retrieves the icon class name for a given currency code.
 *
 * Falls back to `"currency-euro"` if the code is not found.
 *
 * @async
 * @param {string} currencyCode - The ISO 4217 currency code.
 * @returns {Promise<string>} The currency icon name (without the `bi-` prefix).
 *
 * @example
 * const icon = await getCurrencyIcon("JPY");
 * // "currency-yen"
 */
export const getCurrencyIcon = async (currencyCode) => {
    const currencyIcons = await getCurrencyIcons();
    return currencyIcons[currencyCode] ?? "currency-euro";
};

/**
 * Formats a numeric value as a localized currency string.
 *
 * Uses the `toLocaleString` method with the current language (`CURRENT_LANG`)
 * and the given currency code.
 *
 * @param {number} value - The numeric value to format.
 * @param {string} currCode - The ISO 4217 currency code (e.g. `"USD"`, `"EUR"`).
 * @returns {string} The formatted currency string.
 *
 * @example
 * formatCurrency(1234.56, "USD");
 * // "$1,234.56" (in en-US locale)
 */
export const formatCurrency = (value, currCode) => {
    return value.toLocaleString(CURRENT_LANG, {
        style: "currency",
        currency: currCode,
    });
};
