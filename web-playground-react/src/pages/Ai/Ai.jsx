import './Ai.scss';
import { ChatHistory, useChatHistory } from './ChatHistory';
import { ChatInput, useChatInput } from './ChatInput';

const Ai = () => {
	const ollamaUrl = `${import.meta.env.VITE_API_URL}/ai/chat`;
	const chatHistory = useChatHistory();
	const chatInput = useChatInput(handleSendMessage);

	async function handleSendMessage() {
		let message = {
			role: 'user',
			content: chatInput.value
		}
		chatHistory.appendMessage(message);
		chatInput.value = '';
		try {
			chatInput.isLoading = true;
			var response = await postMessages();
			await processResponse(response);
		} finally {
			chatInput.isLoading = false;
		}
	}

	async function postMessages() {
		var payload = {
			model: 'gemma3',
			messages: chatHistory.messages,
			stream: true
		};
		console.log('Posting payload:', payload);
		const response = await fetch(ollamaUrl, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(payload)
		});
		return response;
	}

	async function processResponse(response) {
		if (!response.body) return;
		const reader = response.body.getReader();
		const decoder = new TextDecoder();

		let partialMessage = null;
		while (true) {
			var { value, done } = await reader.read();
			if (done) break;
			var chunk = JSON.parse(decoder.decode(value, { stream: false }));

			if (partialMessage === null) {
				partialMessage = {
					role: chunk.message.role,
					content: chunk.message.content
				}
				chatHistory.appendMessage(partialMessage);
			} else {
				chatHistory.appendLastMessage(chunk.message.content);
			}
		}
	}

	return (
		<div className="ai">
			<h1>Ai</h1>
			<div style={{ display: 'flex', flexDirection: 'column', height: '100%', gap: '10px' }}>
				<ChatHistory instance={chatHistory} />
				<ChatInput instance={chatInput} />
			</div>
		</div>
	);
};

export default Ai;
