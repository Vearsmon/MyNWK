async function fetchLots(pageSize, pageNumber, sortOptions) {
    const params = new URLSearchParams({ pageSize, pageNumber, sortOptions }).toString();
    const response = await fetch(`/baraholka?${params}`, {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
    });

    checkResponse(response);

    return await response.json();
}

function checkResponse(response) {
    if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
    }
}