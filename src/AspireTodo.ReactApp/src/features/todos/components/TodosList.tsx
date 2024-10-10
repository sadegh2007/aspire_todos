import TodoModel from "../models/TodoModel.ts";
import {TodoListItem} from "./TodoListItem.tsx";
import {AnimatePresence} from "framer-motion";
import AnimateTableRowItem from "../../../components/AnimateTableRowItem.tsx";

type TodosListProps = {
    todos: TodoModel[],
    onRemoved: (todo: TodoModel) => void
    onUpdated: (todo: TodoModel) => void
}

const TodosList = (props: TodosListProps) => {
    return (
        <ul className='todos-list mt-3 border border-gray-800 rounded-lg p-3'>
            {
                props.todos.length == 0 && <AnimateTableRowItem><p className='text-center text-gray-600'>There is no todos.</p></AnimateTableRowItem>
            }
            <AnimatePresence initial={false}>
            {
                props.todos.map(todo => <TodoListItem onUpdated={props.onUpdated} key={todo.id} todo={todo} onRemoved={props.onRemoved}/>)
            }
            </AnimatePresence>
        </ul>
    )
}

export default TodosList;