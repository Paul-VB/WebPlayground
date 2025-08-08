import { useState } from 'react';
import { addLoadingOverlay, removeLoadingOverlay  } from 'src/utils/loading';
import './Ai.scss';

const Ai = () => {
    const ollamaUrl = `${import.meta.env.VITE_API_URL}/ai/chat2`;
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
            var message = currentUserMessageInputValue;
            updateUserMessageInputValue(''); 
            const input = document.getElementById('userMessageInputContainer');
            try {
                addLoadingOverlay(input);
                var response = await postMessage(message);
                await processResponse(response);
            } finally {
                removeLoadingOverlay(input);
            }
        }
    }

    return (
        <div className="ai">
            <h1>Ai</h1>
        <div style={{ display: 'flex', flexDirection: 'column', height: '100%', gap: '10px' }}> 
                <div className="chat-container">
                    {chatState}
                </div>
                <div id="userMessageInputContainer" style={{ display: 'inline-block', width: 'auto', alignSelf: 'flex-start' }}> 
					{/* todo: clean this up. make loadihg more universal */}
                    <input
                        type="text"
                        placeholder="Type your message..."
                        value={currentUserMessageInputValue}
                        onChange={e => updateUserMessageInputValue(e.target.value)}
                        onKeyDown={handleSendMessage}
                    />
                </div>
            </div>
        </div>
    );
};

export default Ai;
