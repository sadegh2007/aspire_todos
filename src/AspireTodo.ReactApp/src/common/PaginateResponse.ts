interface PaginateResponse<T> {
    count: number;
    data: T[]
}

export default PaginateResponse;