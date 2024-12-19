//const path = '/testdata/valid.aba';
const path = '/testdata/invalid.aba';
const url = '/api/aba/validate-stream';

window
    .addEventListener(
        "DOMContentLoaded",
        () => {
            const controlDiv
                = document
                    .getElementById("controls");

            createButton(
                controlDiv,
                "Get Data",
                validateAbaStream);

            createButton(
                controlDiv,
                "Get Data Async",
                validateAbaStreamAsync);
        });

async function validateAbaStream() {

    const rawData = await readFile(path);

    let response = await fetch(url,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/octet-stream",
            },
            body: rawData
        });

    if (response.ok) {
        let jsonData = await response.json();

        displayData(...jsonData.map(item => `${item.LineNumber}, ${item.LineContent}, ${item.IsValid}`));
    } else {
        displayData(`Error: ${response.status}: ${response.statusText}`);
    }
}
function displayData(...items) {
    const dataDiv = document.getElementById("data");

    dataDiv.innerHTML = "";

    items.forEach(item => {
        const itemDiv = document.createElement("div");

        itemDiv.innerText = item;
        itemDiv.style.wordwrap = "break-word";

        dataDiv.appendChild(itemDiv);
    });
}
function createButton(parent, label, handler) {
    const button = document.createElement("button");

    button.classList.add("btn", "btn-primary", "m-2");

    button.innerText = label;

    button.onclick = handler;

    parent.appendChild(button);
}

async function validateAbaStreamAsync() {
    const rawData = await readFile(path);

    let response = await fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/octet-stream",
        },
        body: rawData
    });

    clearDataDiv();

    if (response.body) {
        const reader = response.body.getReader();
        const decoder = new TextDecoder("utf-8");

        let partialData = ""; // Buffer for incomplete data chunks

        // Read the stream incrementally
        while (true) {
            const { value, done } = await reader.read();

            if (done) {
                console.log("Stream ended");
                break;
            }

            // Decode the incoming chunk
            partialData += decoder.decode(value, { stream: true });

            // Process complete lines
            let lines = partialData.split("\n");
            partialData = lines.pop(); // Keep the last partial line for next chunk

            for (let line of lines) {
                if (line.trim()) {
                    if (line == "[" || line == "]") continue;
                    try {
                        //console.log(line);
                        line = line.replace(/,$/, "")
                        //console.log(line);

                        // Parse and display each line as it arrives
                        const item = JSON.parse(line);
                        const lineText = `${item.LineNumber}, ${item.LineContent}, ${item.IsValid}`;
                        appendLineToDisplay(lineText);
                        appendListToDisplay(item.ErrorMessages);
                    } catch (e) {
                        console.error("Error parsing line:", line, e);
                    }
                }
            }
        }

        // Display any remaining buffered data
        if (partialData.trim()) {
            try {

                if (partialData != "[" && partialData != "]") {
                    const item = JSON.parse(partialData);
                    const lineText = `${item.LineNumber}, ${item.LineContent}, ${item.IsValid}, ${item.ErrorMessages}`;
                    appendLineToDisplay(lineText);
                    appendListToDisplay(item.ErrorMessages);
                }
            } catch (e) {
                console.error("Error parsing final line:", partialData, e);
            }
        }
    } else {
        displayData(`Error: ${response.status}: ${response.statusText}`);
    }
}

function clearDataDiv() {
    const dataDiv = document.getElementById("data");
    dataDiv.innerHTML = ""; // Clear previous data
}

function appendLineToDisplay(lineText) {
    const dataDiv = document.getElementById("data");
    const itemDiv = document.createElement("div");
    itemDiv.innerText = lineText;
    itemDiv.style.wordWrap = "break-word";
    dataDiv.appendChild(itemDiv);
}
function appendLineToDisplay(lineText) {
    const dataDiv = document.getElementById("data");
    const itemDiv = document.createElement("div");
    itemDiv.innerText = lineText;
    itemDiv.style.wordWrap = "break-word";
    dataDiv.appendChild(itemDiv);
}

function appendListToDisplay(errorMessages) {
    const dataDiv = document.getElementById("data");

    if (!Array.isArray(errorMessages) || errorMessages.length === 0) {
        const noErrorDiv = document.createElement("div");
        noErrorDiv.innerText = "No errors found.";
        noErrorDiv.style.color = "green";
        dataDiv.appendChild(noErrorDiv);
        return;
    }

    const listDiv = document.createElement("div");
    listDiv.classList.add("mt-3");

    const heading = document.createElement("h5");
    heading.classList.add("text-danger");
    heading.innerText = "Errors:";
    listDiv.appendChild(heading);

    const ul = document.createElement("ul");
    ul.classList.add("list-group");

    errorMessages.forEach(error => {
        const li = document.createElement("li");
        li.classList.add("list-group-item", "list-group-item-danger");
        li.innerText = error;
        ul.appendChild(li);
    });

    listDiv.appendChild(ul);
    dataDiv.appendChild(listDiv);
}

async function readFile(fileUrl) {
    try {
        const response = await fetch(fileUrl);

        if (response.ok) {
            const text = await response.text();
            return text; // Return the file content
        } else {
            throw new Error(`Failed to fetch the file: ${response.status} ${response.statusText}`);
        }
    } catch (error) {
        console.error("Error reading the file:", error);
        throw error; // Re-throw the error so the caller can handle it
    }
}
