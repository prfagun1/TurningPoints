﻿function BlazorDownloadFile(filename, contentType, content) {
    // Blazor marshall byte[] to a base64 string, so we first need to convert the string (content) to a Uint8Array to create the File
    const data = base64DecToArr(content);

    // Create the URL
    const file = new File([data], filename, { type: contentType });
    const exportUrl = URL.createObjectURL(file);

    // Create the <a> element and click on it
    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = exportUrl;
    a.download = filename;
    a.target = "_self";
    a.click();

    // We don't need to keep the url, let's release the memory
    // On Safari it seems you need to comment this line... (please let me know if you know why)
    URL.revokeObjectURL(exportUrl);
}

// Convert a base64 string to a Uint8Array. This is needed to create a blob object from the base64 string.
// The code comes from: https://developer.mozilla.org/fr/docs/Web/API/WindowBase64/D%C3%A9coder_encoder_en_base64
function b64ToUint6(nChr) {
    return nChr > 64 && nChr < 91 ? nChr - 65 : nChr > 96 && nChr < 123 ? nChr - 71 : nChr > 47 && nChr < 58 ? nChr + 4 : nChr === 43 ? 62 : nChr === 47 ? 63 : 0;
}

function base64DecToArr(sBase64, nBlocksSize) {
    var
        sB64Enc = sBase64.replace(/[^A-Za-z0-9\+\/]/g, ""),
        nInLen = sB64Enc.length,
        nOutLen = nBlocksSize ? Math.ceil((nInLen * 3 + 1 >> 2) / nBlocksSize) * nBlocksSize : nInLen * 3 + 1 >> 2,
        taBytes = new Uint8Array(nOutLen);

    for (var nMod3, nMod4, nUint24 = 0, nOutIdx = 0, nInIdx = 0; nInIdx < nInLen; nInIdx++) {
        nMod4 = nInIdx & 3;
        nUint24 |= b64ToUint6(sB64Enc.charCodeAt(nInIdx)) << 18 - 6 * nMod4;
        if (nMod4 === 3 || nInLen - nInIdx === 1) {
            for (nMod3 = 0; nMod3 < 3 && nOutIdx < nOutLen; nMod3++, nOutIdx++) {
                taBytes[nOutIdx] = nUint24 >>> (16 >>> nMod3 & 24) & 255;
            }
            nUint24 = 0;
        }
    }
    return taBytes;
}



"use strict";
function clickElement(element) {
    element.click();
}
function downloadFromUrl(options) {
    var _a;
    var anchorElement = document.createElement('a');
    anchorElement.href = options.url;
    anchorElement.download = (_a = options.fileName) !== null && _a !== void 0 ? _a : '';
    anchorElement.click();
    anchorElement.remove();
}
function downloadFromByteArray(options) {
    // The byte array in .NET is encoded to base64 string when it passes to JavaScript.
    // So we can pass that base64 encoded string to the browser as a "data URL" directly.
    var url = "data:" + options.contentType + ";base64," + options.byteArray;
    downloadFromUrl({ url: url, fileName: options.fileName });
}



function FocarElemento(element) {
    if (element instanceof HTMLElement) {
        element.focus();
    }
}


window.moverFocoParaElemento = function (elementId) {
    var elemento = document.getElementById(elementId);
    if (elemento) {
        elemento.focus();
    }
};

