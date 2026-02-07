/**
 * Shows a toast element by ID.
 *
 * @param {string} id - The CSS selector (e.g., `#toastId`) of the toast element.
 *
 * @example
 * showToast("#my-toast");
 */
const showToast = (id) => {
    const toast = document.querySelector(id);
    toast.show();
};

/**
 * Shows the global data error toast.
 *
 * Convenience function for displaying a predefined toast element
 * with the ID `#global-data-error-toast`.
 *
 * @example
 * showDataErrorToast();
 */
export const showDataErrorToast = () => {
    showToast("#global-data-error-toast");
};
