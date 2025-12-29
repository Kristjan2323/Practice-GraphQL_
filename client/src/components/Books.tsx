import {useQuery} from "@apollo/client/react";
import {booksQuery} from "../graphql/queries/booksQueries.ts";

export default function Books(){
    const {loading, data} = useQuery(booksQuery)
    console.log(data)
    if(loading){
        return <div>Loading...</div>
    }
    return(
        <>
        {data?.books.map(book => (
            <h3>Book title: {book.title}</h3>
        ))}
        </>
    )
}