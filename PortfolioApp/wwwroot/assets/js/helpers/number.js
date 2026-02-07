/**
 * Parses a localized numeric string into a JavaScript number.
 *
 * This function takes a string representing a number that may include
 * a custom decimal separator and a thousands separator, then converts
 * it into a proper JavaScript `number`.
 *
 * @param {string} num - The numeric string to parse (e.g. `"1.234,56"`).
 * @param {string} decimal - The decimal separator used in the string (e.g. `","`).
 * @param {string} thousands - The thousands separator used in the string (e.g. `"."`).
 * @returns {number} The parsed number as a float.
 *
 * @example
 * parseNumber("1.234,56", ",", "."); // returns 1234.56
 * parseNumber("12,345.67", ".", ","); // returns 12345.67
 */

export const parseNumber = function (num, decimal, thousands) {
    let bits = num.split(decimal, 2),
        ones = bits[0].replace(new RegExp('\\' + thousands, 'g'), '');
    ones = parseFloat(ones, 10),
        decimal = parseFloat('0.' + bits[1], 10);
    return ones + decimal;
}
