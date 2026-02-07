/**
 * Checks whether a string is null, undefined, or empty (after trimming whitespace).
 *
 * @param {string|null|undefined} string - The string to check.
 * @returns {boolean} `true` if the string is null, undefined, or empty; otherwise `false`.
 *
 * @example
 * stringIsNullOrEmpty(null);        // true
 * stringIsNullOrEmpty("");          // true
 * stringIsNullOrEmpty("   ");       // true
 * stringIsNullOrEmpty("hello");     // false
 */
export const stringIsNullOrEmpty = (string) => {
    return string == null || string == undefined || string.trim() === "";
};

/**
 * Replaces placeholders in a string with corresponding values from an object.
 *
 * Placeholders should be in the format `{key}`. If a key is not found in
 * the replacement object, the placeholder remains unchanged.
 *
 * @param {string|null|undefined} inputString - The string containing placeholders.
 * @param {Object.<string, string>} replacementObject - An object mapping keys to replacement values.
 * @returns {string} The resulting string with placeholders replaced.
 *
 * @example
 * replaceKeysInString("Hello {name}, welcome!", { name: "Alice" });
 * // "Hello Alice, welcome!"
 *
 * replaceKeysInString("Your balance is {balance}", { });
 * // "Your balance is {balance}"
 */
export const replaceKeysInString = (inputString, replacementObject) => {
    if (inputString == null) {
        return "";
    }
    return inputString.replace(/\{(\w+)\}/g, (match, key) => {
        if (replacementObject.hasOwnProperty(key)) {
            return replacementObject[key];
        }
        return match;
    });
};
