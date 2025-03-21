﻿const path2 = '/testdata/invalid.aba';
//const path2 = '/testdata/valid.aba';
const urlv3 = '/api/v3/aba/validate-file';

window
    .addEventListener(
        "DOMContentLoaded",
        () => {
            const controlDiv
                = document
                    .getElementById("controls");

            createButton(
                controlDiv,
                "Validate file Async",
                validateAbaFileAsync2);
        });


async function validateAbaFileAsync2() {

    const rawData = await readFile(path2);
    const correlationId = generateCorrelationId();

    const requestModel = {
        FileName: path2, // Replace with the actual file name
        Base64Content: btoa(rawData)  // Populate this with the byte array of the ABA file
    };

    try {
        const response = await fetch(urlv3, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "X-Correlation-ID": correlationId,
            },
            body: JSON.stringify(requestModel)
        });

        clearDataDiv();

        if (response.ok) {
            try {
                const responseData = await response.json(); // Parse JSON response
                console.log("Response received:", responseData);

                // Validate and display the response
                if (responseData.isValid) {
                    displayData("The ABA file is valid.");
                } else {
                    displayData("The ABA file is invalid. See details below:");
                }

                debugger;

                // Process and display each record validation result
                for (const record of responseData.recordValidationResults) {
                    const lineText = `Line ${record.lineNumber}: Valid=${record.isValid}`;
                    appendLineToDisplay(lineText);

                    if (record.errors && record.errors.length > 0) {
                        appendListToDisplay(record.errors);
                    }

                    if (record.fieldErrors && record.fieldErrors.length > 0) {

                        for (const field of record.fieldErrors) {
                            const fLineText = `Field : Valid=${field.isValid}`;
                            appendLineToDisplay(fLineText);

                            appendListToDisplay(field.errors)
                        }
                    }
                }
            } catch (error) {
                console.error("Error processing response:", error);
                displayData("Failed to process the server response.");
            }
        } else {
            displayData(`Error: ${response.status}: ${response.statusText}`);
        }


    } catch (error) {
        console.error("Error during request:", error);
    }
}
