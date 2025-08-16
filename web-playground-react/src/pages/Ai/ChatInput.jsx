import { PIANO } from 'src/utils/piano';

const ChatInput = ({ instance }) => {
	function handleKeyDown(event) {
		if (event.key === 'Enter' && !event.shiftKey) {
			event.preventDefault();
			instance.onSend();
		}
	}
	return (
		<div>
			<div className={`loading-container ${instance.isLoading ? 'loading' : ''}`}>
				<textarea
					placeholder="Type your message..."
					value={instance.value}
					onChange={e => instance.value = e.target.value}
					onKeyDown={handleKeyDown}
					rows={3}
				/>
			</div>
			<div>
				<input
					type="checkbox"
					checked={instance.shouldStream}
					onChange={e => instance.shouldStream = e.target.checked}
					id="shouldStreamCheckbox"
				/>
				<label htmlFor="shouldStreamCheckbox">Stream response</label>
			</div>
		</div>
	);
};

function useChatInput(onSend) {
	return PIANO({
		value: '',
		isLoading: false,
		shouldStream: true,
		onSend: onSend,
	});
}

export { ChatInput, useChatInput };
