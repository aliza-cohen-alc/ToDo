import axios from 'axios';

const apiUrl = "http://localhost:5044/"

export default {
  getTasks: async () => {
    const result = await axios.get(`${apiUrl}a`)    
    return result.data;
  },

  addTask: async(name)=>{
    console.log('addTask', name)
    const task = await axios.post(`${apiUrl}`,{name,isComplete:false})
    return task.data;
  },

  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
    const task = await axios.put(`${apiUrl}${id}`,{isComplete})
    return task.data;;
  },

  deleteTask:async(id)=>{
    console.log('deleteTask')
    const task = await axios.delete(`${apiUrl}${id}`)
    return task.data;
  }
};
