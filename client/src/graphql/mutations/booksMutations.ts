import {gql} from "@apollo/client";

export const addBookMutation = gql`
  mutation AddBook($input: BookInput!) {
    addBook(input: $input) {
      book {
        id
        title
        pages
      }
    }
  }
`;