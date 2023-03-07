import { MultiSelect } from "./components/MultiSelect";

//function App() {
//        return <MultiSelect />;
//    }

//ReactDOM.render(
//    <React.StrictMode>
//        <App />
//    </React.StrictMode>,
//    document.getElementById('root')
//);


var Hello = React.createClass({
    render: function () {
        return (
            <div>Hello! test react...</div>
        );
    }
});

React.render(<Hello />, document.getElementById("reactdiv"));
