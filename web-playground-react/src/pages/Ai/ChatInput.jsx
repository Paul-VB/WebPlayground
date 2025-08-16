import { PIANO } from 'src/utils/piano';
import { addLoadingOverlay, removeLoadingOverlay } from 'src/utils/loading';

const ChatInput = ({ instance }) => {
	function handleKeyDown(event) {
		if (event.key === 'Enter') {
			instance.onSend();
		}
	}
	return (
		<div>
			<input
				type="text"
				placeholder="Type your message..."
				value={instance.value}
				onChange={e => instance.value = e.target.value}
				onKeyDown={handleKeyDown}
			/>
		</div>
	);
};

function useChatInput(onSend) {
	return PIANO({
		value: '',
		onSend: onSend,
		addLoadingOverlay: function () {
			//todo
		},
		removeLoadingOverlay: function () {
			//todo
		}
	});
}

class ChatInput {
    value = '';
    onSend = null;
    
    constructor(onSend) {
        this.onSend = onSend;
    }
    
    addLoadingOverlay() {
        // todo
    }
    
    removeLoadingOverlay() {
        // todo
    }
}


export { ChatInput, useChatInput };
