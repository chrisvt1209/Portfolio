/**
 * Retrieves the value of a query parameter from the current URL.
 *
 * @param {string} key - The name of the query parameter to get.
 * @returns {string|null} The value of the query parameter, or `null` if it does not exist.
 *
 * @example
 * // URL: https://example.com?page=2
 * getQueryParameter("page"); // "2"
 */
export const getQueryParameter = (key) => {
    const search = new URL(window.location).search;
    const params = new URLSearchParams(search);
    return params.get(key);
};

/**
 * Adds or updates a query parameter in the current URL without reloading the page.
 *
 * @param {string} key - The name of the query parameter.
 * @param {string|number} value - The value to set for the query parameter.
 *
 * @example
 * setQueryParameter("page", 3);
 * // Updates URL: https://example.com?page=3
 */
export const setQueryParameter = (key, value) => {
    const uri = window.location.href;
    const re = new RegExp(`([?&])${key}=.*?(&|$)`, "i");
    const separator = uri.indexOf("?") !== -1 ? "&" : "?";
    let result;

    if (uri.match(re)) {
        result = uri.replace(re, `$1${key}=${value}$2`);
    } else {
        result = uri + separator + key + "=" + value;
    }

    window.history.pushState({}, null, result);
};

/**
 * Removes a query parameter from the current URL without reloading the page.
 *
 * @param {string} key - The name of the query parameter to remove.
 *
 * @example
 * removeQueryParameter("page");
 * // URL no longer contains "page" parameter
 */
export const removeQueryParameter = (key) => {
    const url = window.location.href;
    const result = url
        .replace(new RegExp(`[?&]${key}=[^&#]*(#.*)?$`), "$1")
        .replace(new RegExp(`([?&])${key}=[^&]*&`), "$1");

    window.history.pushState({}, null, result);
};
