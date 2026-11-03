/**
 *
 * @param url {URL|RequestInfo}
 * @return {Promise<Response>}
 */
export const get = async (url) => {
    const response = await fetch(url, {
        method: "GET",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json",
            "Tmb-Lang": CURRENT_LANG,
            "Tmb-Is-Ajax": "1",
        }
    });

    if (response.status === 401) {
        redirectToLoginPage();
    }

    return response;
}

/**
 *
 * @param url {URL|RequestInfo}
 * @param body {object}
 * @return {Promise<Response>}
 */
export const post = async (url, body) => {
    const response = await fetch(url, {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json",
            "Tmb-Lang": CURRENT_LANG,
            "Tmb-Is-Ajax": "1",
        },
        body: JSON.stringify(body)
    });

    if (response.status === 401) {
        redirectToLoginPage();
    }

    return response;
}

const redirectToLoginPage = () => {
    window.location.href = `${LOGIN_URL}?returnUrl=${window.location.pathname}`;
}
