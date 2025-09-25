let webSocket = null;
let onReceiveHandlers = [];
let pendingMessages = {};

const wsUrl = import.meta.env.VITE_API_URL.replace(/^https?/, 'wss');
const receiveTimeoutMilliseconds = 30000;
async function initialize() {

	try {
		webSocket = new WebSocket(`${wsUrl}/WebSocket/initialize`);
	}
	catch (error) {
		console.error('Failed to create WebSocket:', error);
	}

	webSocket.addEventListener('message', (event) => {
		let message = JSON.parse(event.data);
		handleMessage(message);
	});

}

function handleMessage(message) {
	let messageIsRequestToClient = !!message.eventType;
	if (messageIsRequestToClient) {
		HandleRequestToClient(message);
	} else {
		HandleResponseFromServer(message);
	}
}

function HandleRequestToClient(message) {
	for (let onReceiveHandler of onReceiveHandlers) {
		if (onReceiveHandler.eventType === message.eventType) {
			let requestPayload = JSON.parse(message.payload) ?? "";
			let responsePayload = onReceiveHandler.handler(requestPayload) ?? "{success: true}";
			let responseMessage = {
				id: message.id,
				payload: responsePayload
			};
			console.log('Sending response message:', responseMessage);
			let responseMessageJson = JSON.stringify(responseMessage);
			webSocket.send(responseMessageJson);
		}
	}
}

function HandleResponseFromServer(message) {
	//todo: manually review this 
	if (!pendingMessages[message.id]) {
		throw new Error(`There is no pending message with the id: ${message.id}`);
	}
	let resolver = pendingMessages[message.id];
	resolver(message);
}

async function send(eventType, payload) {
	if (!webSocket || webSocket.readyState !== WebSocket.OPEN) {
		throw new Error("WebSocket is not open");
	}

	let message = {
		id: crypto.randomUUID(),
		eventType,
		payload
	};
	let messageJson = JSON.stringify(message);

	let pendingMessageResponse = new Promise((resolve) => {
		pendingMessages[message.id] = resolve;
	});

	let timeoutPromise = new Promise((resolve, reject) => {
		setTimeout(() => {
			let error = new Error(`Timeout waiting for response to message with id: ${message.id}`);
			error.reason = "timeout";
			reject(error);
		}, receiveTimeoutMilliseconds);
	});

	webSocket.send(messageJson);

	try {
		let response = await Promise.race([pendingMessageResponse, timeoutPromise]);
		return response.payload;
	} catch (error) {
		if (error.reason === "timeout") {
			//todo: handle timeout specifically.
		}
		throw error;
	} finally {
		delete pendingMessages[message.id];
	}
}

async function onReceive(eventType, handler) {
	onReceiveHandlers.push({ eventType, handler });
}

export default { initialize, send, onReceive };