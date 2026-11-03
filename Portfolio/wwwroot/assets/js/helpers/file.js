/**
 * Converts a File object to a Base64-encoded data URL.
 *
 * Uses a FileReader to read the file asynchronously and returns
 * a Promise that resolves with the Base64 string.
 *
 * @param {File} file - The file to convert.
 * @returns {Promise<string|ArrayBuffer|null>} A Promise that resolves with the Base64 data URL.
 *
 * @example
 * const base64 = await toBase64(fileInput.files[0]);
 */
export const toBase64 = (file) => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = reject;
});

/**
 * Converts a data URL or a remote URL into a File object.
 *
 * - If the URL is a data URL, it decodes it and creates a File.
 * - If the URL is a remote URL, it fetches it as an ArrayBuffer and creates a File.
 *
 * @param {string} url - The data URL or remote URL of the file.
 * @param {string} filename - The name for the resulting File object.
 * @param {string} [mimeType] - The MIME type to use if not inferable from the data URL.
 * @returns {Promise<File>} A Promise that resolves with the created File object.
 *
 * @example
 * // From a data URL
 * const file = await toFile("data:image/png;base64,...", "image.png");
 *
 * // From a remote URL
 * const file = await toFile("https://example.com/image.png", "image.png", "image/png");
 */
export const toFile = (url, filename, mimeType) => {
    if (url.startsWith('data:')) {
        var arr = url.split(','),
            mime = arr[0].match(/:(.*?);/)[1],
            bstr = atob(arr[arr.length - 1]),
            n = bstr.length,
            u8arr = new Uint8Array(n);
        while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
        }
        var file = new File([u8arr], filename, { type: mime || mimeType });
        return Promise.resolve(file);
    }

    return fetch(url)
        .then(res => res.arrayBuffer())
        .then(buf => new File([buf], filename, { type: mimeType }));
};
