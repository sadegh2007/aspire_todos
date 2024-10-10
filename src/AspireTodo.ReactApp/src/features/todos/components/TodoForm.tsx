import {z} from "zod";
import {useState} from "react";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import UpsertTodoRequest from "../requests/UpsertTodoRequest.ts";
import {catchError} from "../../../services/ErrorHandlers.ts";
import {CreateTodoApiRequest, UpdateTodoApiRequest} from "../services/TodoService.ts";
import TodoModel from "../models/TodoModel.ts";
import PredictiveText from "../../../components/predictiveText";

const TodoSchema = z.object({
    title: z.string().min(1),
    summery: z.string().min(3).max(400).optional()
});

type TodoSchemaType = z.infer<typeof TodoSchema>;

type TodoFormProps = {
    onCreated?: (todo: TodoModel) => void, 
    onUpdated?: (todo: TodoModel) => void, 
    onCancelEdit?: () => void,
    isEdit?: boolean, 
    todo?: TodoModel
}

const TodoForm = (props: TodoFormProps) => {
    const [loading, setLoading] = useState<boolean>(false);
    const [summery, setSummery] = useState<string|undefined>();

    const {
        register,
        handleSubmit,
        reset,
        formState: {errors},
    } = useForm<TodoSchemaType>({
        resolver: zodResolver(TodoSchema),
        defaultValues: props.todo ?? {}
    });
    
    const onSubmit = async (values: UpsertTodoRequest) => {
        setLoading(true);
        values.summery = summery;
        
        try {
            if (props.isEdit) {
                await UpdateTodoApiRequest(props.todo!.id, values);
                
                props.onUpdated!({
                    ...props.todo!,
                    title: values.title,
                    summery: values.summery,
                } as TodoModel)
            }
            else {
                const todo = await CreateTodoApiRequest(values);
                props.onCreated!(todo);
            }
            
            reset();
            setSummery(undefined);
        }
        catch (e) {
            catchError(e)
        }
        
        setLoading(false);
    }
    
    return (
        <form onSubmit={handleSubmit(onSubmit)} className="">
            <div className="flex w-full mb-2">
                <label className="form-control w-full">
                    <input
                        disabled={loading}
                        {...register('title')}
                        type="text"
                        placeholder="type the task title..."
                        className={`focus:outline-0 input ${errors.title ? 'input-error' : ''} w-full`}/>
                    {
                        errors.title &&
                        <div className="label">
                            <span className="label-text-alt">{errors.title.message}</span>
                        </div>
                    }
                </label>
                <button disabled={loading} className='btn btn-primary ml-2'>{
                    loading ? <span className='loading loading-spinner'/> : props.isEdit ? 'Update' : 'Add'
                }</button>
                {
                    props.isEdit &&
                    <button type="button" onClick={props.onCancelEdit!} disabled={loading} className='btn btn-outline btn-error ml-2'>
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none"
                             stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"
                             className="icon icon-tabler icons-tabler-outline icon-tabler-x">
                            <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                            <path d="M18 6l-12 12"/>
                            <path d="M6 6l12 12"/>
                        </svg>
                    </button>
                }
            </div>

            <PredictiveText
                placeholder="Summery..."
                value={props.todo?.summery}
                onChange={setSummery}
            />
            {/*<label className="form-control w-full mt-2">*/}
            {/*    <input*/}
            {/*        disabled={loading}*/}
            {/*        {...register('summery')}*/}
            {/*        type={"text"}*/}
            {/*        maxLength={400}*/}
            {/*        placeholder="summery..."*/}
            {/*        className={`focus:outline-0 input ${errors.summery ? 'input-error' : ''} w-full`}/>*/}
            {/*    {*/}
            {/*        errors.summery &&*/}
            {/*        <div className="label">*/}
            {/*            <span className="label-text-alt">{errors.summery.message}</span>*/}
            {/*        </div>*/}
            {/*    }*/}
            {/*</label>*/}
        </form>
    )
}

export default TodoForm;