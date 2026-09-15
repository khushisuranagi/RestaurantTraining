window.renderDocxPreview = async (elementId, base64Data) => {
    const container = document.getElementById(elementId);
    if (!container) return;

    try {
        const binary = atob(base64Data);
        const bytes = new Uint8Array(binary.length);
        for (let i = 0; i < binary.length; i++) {
            bytes[i] = binary.charCodeAt(i);
        }

        const result = await mammoth.convertToHtml({ arrayBuffer: bytes.buffer });
        container.innerHTML = result.value;
    } catch (err) {
        container.innerHTML = "<p>Could not preview this document.</p>";
        console.error(err);
    }
};