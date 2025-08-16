import { PIANO } from 'src/utils/piano';
import 'github-markdown-css';
import './ChatHistory.scss';
import ReactMarkdown from 'react-markdown';
const ChatHistory = ({ instance }) => {
	return (
		<div className="chat-container">
			{instance.messages.map((msg, index) => (
				<div key={index}>
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
	return PIANO({
		messages: [],
		appendMessage: function (message) {
			this.messages.push(message);
		},
		appendLastMessage: function (newContent) {
			const lastMessage = this.messages[this.messages.length - 1];
			lastMessage.content += newContent;
		}
	});
}
export { ChatHistory, useChatHistory };