import { format, isValid, parseISO } from "date-fns";

/**
 * Formats a date string into a human-readable format.
 *
 * Converts the given date string into a `Date` object and formats it
 * using the globally defined `DATE_FORMAT`.
 *
 * @param {string} dateString - A valid date string (ISO 8601 or any format recognized by `Date`).
 * @returns {string} The formatted date string.
 *
 * @example
 * // Assuming DATE_FORMAT = "dd/MM/yyyy"
 * formatDateString("2025-09-24"); // "24/09/2025"
 */
export const formatDateString = (dateString) => {
    const date = new Date(dateString);
    return format(date, DATE_FORMAT);
};

/**
 * Attempts to parse an ISO date string into a `Date` object.
 *
 * Uses `date-fns/parseISO` for parsing and validates the result.
 * If parsing fails or the date is invalid, returns `false`.
 *
 * @param {string} dateString - The ISO date string to parse.
 * @returns {Date|false} A valid `Date` object if parsing succeeds, otherwise `false`.
 *
 * @example
 * tryParseDate("2025-09-24"); 
 * // Returns: Date object for 24 Sept 2025
 *
 * tryParseDate("invalid-date");
 * // Returns: false
 */
export const tryParseDate = (dateString) => {
    try {
        const parsedDate = parseISO(dateString);

        if (!isValid(parsedDate))
            return false;

        format(parsedDate, "yyyy-MM-dd");

        return parsedDate;
    } catch (err) {
        return false;
    }
};
