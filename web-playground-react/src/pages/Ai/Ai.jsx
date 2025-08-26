import './Ai.scss';
import 'src/styles/page.scss';
import AiModelSelector, { useAiModelSelector } from './AiModelSelector';
import ChatHistory, { useChatHistory } from './ChatHistory';
import ChatInput, { useChatInput } from './ChatInput';
import { ReadNdjsonStream } from 'src/utils/ndjsonStreamReader';

const Ai = () => {
	const ollamaUrl = `${import.meta.env.VITE_API_URL}/ai/chat`;
	const chatHistory = useChatHistory();
	const chatInput = useChatInput(handleSendMessage);
	const aiModelSelector = useAiModelSelector();

	async function handleSendMessage() {
		let message = {
			role: 'user',
			content: chatInput.value
		}
		chatHistory.appendMessage(message);
		chatInput.setValue('');
		try {
			chatInput.setIsLoading(true);
			var response = await postMessages();
			await processResponse(response);
		} catch (error) {
			console.error('Error sending message:', error);
		} finally {
			chatInput.setIsLoading(false);
		}
	}
	async function postMessages() {
		var payload = {
			model: aiModelSelector.selectedModel,
			messages: chatHistory.getMessages(),
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
		<div className="ai page">
			<div className='page-header'>
				<h1>Ai</h1>
			</div>
			<div className='page-body'>
				<ChatHistory instance={chatHistory} />
			</div>
			<div className='page-footer'>
				<ChatInput instance={chatInput} />
				<AiModelSelector instance={aiModelSelector} />
			</div>
		</div>
	);
};

export default Ai;
