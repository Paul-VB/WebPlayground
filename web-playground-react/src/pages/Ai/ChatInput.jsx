import { useState } from 'react';
import './ChatInput.scss';

const ChatInput = ({ instance }) => {
	function handleKeyDown(event) {
		if (event.key === 'Enter' && !event.shiftKey) {
			event.preventDefault();
			instance.onSend();
		}
	}
	return (
		<div className="chat-input">
			<div className={`chat-input-textbox-container loading-container ${instance.isLoading ? 'loading' : ''}`}>
				<textarea className="chat-input-textbox"
					placeholder="Type your message..."
					value={instance.value}
					onChange={e => instance.setValue(e.target.value)}
					onKeyDown={handleKeyDown}
					rows={3}
				/>
			</div>
			<button
				className="chat-send-button"
				onClick={() => instance.onSend()}
				disabled={instance.isLoading || !instance.value.trim()}
			>
				→
			</button>
			<div className="chat-options">
				<input
					type="checkbox"
					checked={instance.shouldStream}
					onChange={e => instance.setShouldStream(e.target.checked)}
					id="shouldStreamCheckbox"
				/>
				<label htmlFor="shouldStreamCheckbox">Stream response</label>
			</div>
		</div>
	);
};

function useChatInput(onSend) {
	const [value, setValue] = useState('');
	const [isLoading, setIsLoading] = useState(false);
	const [shouldStream, setShouldStream] = useState(true);
	return { value, setValue, isLoading, setIsLoading, shouldStream, setShouldStream, onSend };
}

export default ChatInput;
export { useChatInput };
