import {gql} from '@apollo/client'

export const booksQuery = gql `
query {
books{
id
title
pages
author{
name
}
}
}
`