import { useEffect, useRef } from 'react';
import 'github-markdown-css';
import './ChatHistory.scss';
import ReactMarkdown from 'react-markdown';
import useGetState from 'src/utils/useGetState';
const ChatHistory = ({ instance }) => {
	instance.ref = useRef(null);

	useEffect(() => {
		if (instance.ref.current) {
			instance.ref.current.scrollTop = instance.ref.current.scrollHeight;
		}
	}, [instance.messages]);

	return (
		<div className="chat-history-container" ref={instance.ref}>
			{instance.getMessages().map((msg, index) => (
				<div key={index} className="chat-message-container" data-role={msg.role}>
					<strong>{msg.role}:</strong>

					<div className="markdown-body">
						<ReactMarkdown>{msg.content}</ReactMarkdown>
					</div>
				</div>
			))}
		</div>
	);
};

function useChatHistory() {
	const [messages, setMessages, getMessages] = useGetState([]);
	function appendMessage(message) {
		setMessages(prev => [...prev, message]);
	}
	function appendLastMessage(newContent) {
		setMessages(prev => {
			if (prev.length === 0) return prev;

			const updatedMessages = [...prev];
			// Copy the last message and update its content
			const lastIndex = updatedMessages.length - 1;
			const lastMessage = { ...updatedMessages[lastIndex] };
			lastMessage.content += newContent;

			// Replace the last message in the array
			updatedMessages[lastIndex] = lastMessage;
			return updatedMessages;
		});
	}
	return { messages, setMessages, getMessages, appendMessage, appendLastMessage };
}
export default ChatHistory;
export { useChatHistory };