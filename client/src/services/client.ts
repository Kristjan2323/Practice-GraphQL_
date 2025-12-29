import {ApolloClient, InMemoryCache, ApolloLink, HttpLink} from '@apollo/client'
import {ErrorLink} from "@apollo/client/link/error";

const errorLink = new ErrorLink(({error}) => {
    if(error){
        alert(error.message)
    }
})

const apolloLik = ApolloLink.from([
    errorLink,
    new HttpLink({uri:'http://localhost:5034/graphql/'}),
])

export const client  = new ApolloClient({
    link: apolloLik,
    cache:new InMemoryCache()
})