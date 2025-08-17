import './Ai.scss';
import { ChatHistory, useChatHistory } from './ChatHistory';
import { ChatInput, useChatInput } from './ChatInput';
import { ReadNdjsonStream } from 'src/utils/ndjsonStreamReader';

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
			stream: chatInput.shouldStream
		};
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
		
		let partialMessage = null;
		for await (const chunk of ReadNdjsonStream(response.body)) {
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
