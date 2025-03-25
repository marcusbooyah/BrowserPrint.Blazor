export function fetchJson(url) {
    return fetch(url).then(response => response.json());
}

export function fetchText(url, data) {
    return fetch(url).then(response => response.text());
}

export function postJson(url, data) {
    return fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }).then(response => response.json());
}

export function postText(url, data) {
    return fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }).then(response => response.text());
}