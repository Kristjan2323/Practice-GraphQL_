import React, { useState } from 'react';
import { useMutation } from '@apollo/client/react';
import { addBookMutation } from '../graphql/mutations/booksMutations';

const AddBookForm = () => {
  const [formData, setFormData] = useState({
    title: '',
    pages: '',
    authorId: ''
  });

  const [addBook, { data, loading, error }] = useMutation(addBookMutation)
  console.log("🚀 ~ AddBookForm ~ loading:", loading)

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));

  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    addBook({
      variables: {
        input: {
          title: formData.title,
          authorId: '694eec5e5b64a8be293c3c59',
          pages: parseInt(formData.pages)
        }
        
      }
    })
    console.log('data', data)

    if (error) {
      console.error('graph err', error)
    }
  };

  return (
    <div className="add-book-form">
      <h2>Add New Book</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="title">Title:</label>
          <input
            type="text"
            id="title"
            name="title"
            value={formData.title}
            onChange={handleChange}
            placeholder="Enter book title"
          />
        </div>

        <div className="form-group">
          <label htmlFor="pages">Pages:</label>
          <input
            type="number"
            id="pages"
            name="pages"
            value={formData.pages}
            onChange={handleChange}
            placeholder="Enter number of pages"
          />
        </div>

        <div className="form-group">
          <label htmlFor="authorId">Author ID:</label>
          <input
            type="text"
            id="authorId"
            name="authorId"
            value={formData.authorId}
            onChange={handleChange}
            placeholder="Enter author ID"
          />
        </div>

        <button type="submit">Add Book</button>
      </form>
    </div>
  );
};

export default AddBookForm;
