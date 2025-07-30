import { useState } from 'react';

const Ai = () => {
    const ollamaIp = import.meta.env.VITE_OLLAMA_IP_ADDRESS;
    const [currentUserMessageInputValue, updateUserMessageInputValue] = useState('');
	const [chatState, updateChatState] = useState('');

    async function postMessage(message) {
		var payload = {
			model: 'gemma3',
			messages: [{
				role: 'user',
				content: message
			}]
		};

        const response = await fetch(`${ollamaIp}/api/chat`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload) // <-- Fix: stringify the payload
        });
        return await response;
    }

    async function processResponse(response) {
        if (!response.body) return;
        const reader = response.body.getReader();
        const decoder = new TextDecoder();

		appendToChatState('AI:', false); // Add a label for the assistant's response

		while (true) {
			var { value, done } = await reader.read();
			if (done) break;
			var chunk = JSON.parse(decoder.decode(value, { stream: true }));
			appendToChatState(chunk.message.content, false); // Append chunk without newline
		}
		appendToChatState('', true); // Ensure a newline after the AI response
    }

	function appendToChatState(message, newline = true) {
		updateChatState(prevState => prevState + message + (newline ? '\n' : ''));
	}

    async function handleSendMessage(event) {
        if (event.key === 'Enter' && document.activeElement === event.currentTarget) {
			appendToChatState(`User: ${currentUserMessageInputValue}`);
			var response = await postMessage(currentUserMessageInputValue);
			updateUserMessageInputValue(''); 
			processResponse(response);
        }
    }

    return (
        <div>
            <h1>Ai</h1>
            <div>Ollama IP: {ollamaIp}</div>
            <div style={{ border: '1px solid #fff', minHeight: '100px', whiteSpace: 'pre-wrap' }}>
                {chatState}
            </div>
            <input
                type="text"
                placeholder="Type your message..."
                value={currentUserMessageInputValue}
                onChange={e => updateUserMessageInputValue(e.target.value)}
                onKeyDown={handleSendMessage}
            />
        </div>
    );
};

export default Ai;
