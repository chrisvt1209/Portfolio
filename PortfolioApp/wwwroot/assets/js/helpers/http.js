/**
 * Sends a GET request to the specified URL with JSON headers.
 *
 * @async
 * @param {string} url - The URL to send the GET request to.
 * @returns {Promise<Response>} A Promise that resolves with the Fetch API Response object.
 *
 * @example
 * const response = await get("/api/users");
 * const data = await response.json();
 */
export const get = async (url) => {
    return fetch(url, {
        method: "GET",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json",
            "Tmb-Lang": CURRENT_LANG,
        }
    });
};

/**
 * Sends a POST request with a JSON body to the specified URL.
 *
 * @async
 * @param {string} url - The URL to send the POST request to.
 * @param {Object} body - The JavaScript object to send as the JSON body.
 * @returns {Promise<Response>} A Promise that resolves with the Fetch API Response object.
 *
 * @example
 * const response = await post("/api/users", { name: "Alice", age: 25 });
 * const data = await response.json();
 */
export const post = async (url, body) => {
    return fetch(url, {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json",
            "Tmb-Lang": CURRENT_LANG,
        },
        body: JSON.stringify(body)
    });
};
