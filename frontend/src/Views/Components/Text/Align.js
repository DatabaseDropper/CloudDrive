import React from 'react';

const Component_Text_Align = {
    Center: (props) => {
        return (<div style={{textAlign: 'center'}}>{props.children}</div>);
    }
}

export default Component_Text_Align;