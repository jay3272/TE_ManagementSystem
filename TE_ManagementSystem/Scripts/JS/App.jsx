var Hello = React.createClass({
    render: function () {
        return (
            <div>Hello! test react...</div>
            );
    }
});

React.render(<Hello />, document.getElementById("reactdiv"));
