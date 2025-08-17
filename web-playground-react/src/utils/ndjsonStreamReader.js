async function* ReadNdjsonStream(stream) {

	const reader = stream.getReader();
	const decoder = new TextDecoder();
	let buffer = ""

	function tryParseLine(line) {
		if (!line.trim()) return null;

		try {
			let object = JSON.parse(line);
			return object;
		} catch (error) {
			console.error('Error parsing line:', error, '\nLine content:', line);
			return null; // Skip this line if parsing fails
		}
	}

	while (true) {
		let { value, done } = await reader.read();
		if (done) break;

		let chunkJson = decoder.decode(value, { stream: false });
		buffer += chunkJson;

		let lines = buffer.split('\n');
		let potentiallyIncompleteLine = lines.pop();
		buffer = potentiallyIncompleteLine;

		for (const line of lines) {
			let object = tryParseLine(line);
			if (object) {
				yield object;
			}
		}
	}
	let finalObject = tryParseLine(buffer);
	if (finalObject) {
		yield finalObject;
	}
}

export { ReadNdjsonStream };