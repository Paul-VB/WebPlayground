import { PIANO } from 'src/utils/piano';

const ChatInput = ({ instance }) => {
	function handleKeyDown(event) {
		if (event.key === 'Enter' && !event.shiftKey) {
			event.preventDefault();
			instance.onSend();
		}
	}
	return (
<div className={`loading-container ${instance.isLoading ? 'loading' : ''}`}>
			<textarea
				placeholder="Type your message..."
				value={instance.value}
				onChange={e => instance.value = e.target.value}
				onKeyDown={handleKeyDown}
				rows={3}
			/>
		</div>
	);
};

function useChatInput(onSend) {
	return PIANO({
		value: '',
		isLoading: false,
		onSend: onSend,
	});
}

export { ChatInput, useChatInput };
