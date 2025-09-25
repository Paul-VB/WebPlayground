import initSqlJs from 'sql.js';
import webSocketService from './webSocketService';

let SQL = null;
let currentDb = null;
const SQLJS_DB_PREFIX = 'sqljs_db_';
const getLocalStorageKey = (dbName) => `${SQLJS_DB_PREFIX}${dbName}`;

async function initConnection() {
	await webSocketService.initialize();
	SQL = await initSqlJs({
		locateFile: file => `https://sql.js.org/dist/${file}`
	});

	webSocketService.onReceive('loadDb', loadDb);
	webSocketService.onReceive('saveDb', saveDb);
	webSocketService.onReceive('createDbIfNotExists', createDbIfNotExists);
	webSocketService.onReceive('executeQuery', executeQuery);
	webSocketService.onReceive('executeNonQuery', executeNonQuery);
}

function loadDb(payload) {
	const dbName = payload.dbName;
	const dataJson = localStorage.getItem(getLocalStorageKey(dbName));
	if (dataJson) {
		const data = new Uint8Array(JSON.parse(dataJson));
		const db = new SQL.Database(data);
		db.dbName = dbName;
		currentDb = db;
	}
}

function saveDb(db = currentDb) {
	if (!db.dbName) {
		throw new Error('Database must have a name to be saved. Set db.dbName property.');
	}
	const dbData = db.export();
	let dbJson = JSON.stringify(Array.from(dbData))
	localStorage.setItem(getLocalStorageKey(db.dbName), dbJson);
};

function createDbIfNotExists(payload) {
	console.log('createDbIfNotExists', payload); //remove later
	const dbName = payload.dbName;
	const dataJson = localStorage.getItem(getLocalStorageKey(dbName));
	if (dataJson) return;
	const db = new SQL.Database();
	db.dbName = dbName;
	saveDb(db);
}

function executeQuery(payload) {
	const sql = payload.sql;
	const parameters = payload.parameters || [];
	if (!currentDb) {
		throw new Error('No database loaded');
	}

	const results = currentDb.exec(sql, parameters);

	//todo test this and make the format make sense
	return {
		payload: JSON.stringify({
			columns: results[0]?.columns || [],
			values: results[0]?.values || []
		})
	};
}

function executeNonQuery(payload) {
	const sql = payload.sql;
	const parameters = payload.parameters || [];
	if (!currentDb) {
		throw new Error('No database loaded');
	}

	const results = currentDb.run(sql, parameters);

	return {
		rowsAffected: currentDb.getRowsModified()
	};
}



export default { initConnection };