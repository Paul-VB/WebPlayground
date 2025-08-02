import { useState } from 'react';

const Ai = () => {
    const ollamaUrl = `${import.meta.env.VITE_API_URL}/ai/chat`;
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

        const response = await fetch(ollamaUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        });
        return await response;
    }

    async function processResponse(response) {
        if (!response.body) return;
        const reader = response.body.getReader();
        const decoder = new TextDecoder();

        appendToChatState('AI: ', false); 

        while (true) {
            var { value, done } = await reader.read();
            if (done) break;
            var chunkStr = decoder.decode(value, { stream: true });
            var lines = chunkStr.split('\n');
            for (let line of lines) {
                if (line.trim() === '') continue;
                try {
                    var chunk = JSON.parse(line);
                    appendToChatState(chunk.message.content, false);
                } catch (e) {
                    console.log('Failed to parse part of stream response:', line);
                }
            }
        }
        appendToChatState('', true); 
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
