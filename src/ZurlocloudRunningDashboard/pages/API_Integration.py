import requests
import streamlit as st

st.set_page_config(layout="wide")

@st.cache_data
def get_stores():
    """Return a list of hotels from the API."""
    print("endpoint: ",st.secrets["api"]["endpoint"])
    api_endpoint = st.secrets["api"]["endpoint"]
    response = requests.get(f"{api_endpoint}/Stores", timeout=10)
    return response

@st.cache_data
def get_store_products(store_id):
    """Return a list of products for the specified hotel."""
    api_endpoint = st.secrets["api"]["endpoint"]
    response = requests.get(f"{api_endpoint}/Stores/{store_id}/Products", timeout=10)
    return response

@st.cache_data
def invoke_chat_endpoint(question):
    """Invoke the chat endpoint with the specified question."""
    api_endpoint = st.secrets["api"]["endpoint"]
    response = requests.post(f"{api_endpoint}/Chat", data={"message": question}, timeout=10)
    return response

def main():
    """Main function for the Chat with Data Streamlit app."""

    st.write(
    """
    # API Integration via Semantic Kernel

    This Streamlit dashboard is intended to demonstrate how we can use
    the Semantic Kernel library to generate SQL statements from natural language
    queries and display them in a Streamlit app.

    ## Select a Store
    """
    )

    # Display the list of hotels as a drop-down list
    stores_json = get_stores().json()
    # Reshape hotels to an object with hotelID and hotelName
    stores = [{"id": store["storeID"], "name": "Zurlocloud Running - " + store["city"]} for store in stores_json]
    print(stores)
    
    selected_store = st.selectbox("Store:", stores, format_func=lambda x: x["name"])

    # Display the list of orders for the selected hotel as a table
    if selected_store:
        print(selected_store)
        store_id = selected_store["id"]
        products = get_store_products(store_id).json()
        print(products)
        st.write("### Products")
        st.table(products)

    st.write(
        """
        ## Ask a Store or Product Question

        Enter a question about the store or their products in the text box below.
        Then select the "Submit" button to call the Chat endpoint.
        """
    )

    question = st.text_input("Question:", key="question")
    if st.button("Submit"):
        with st.spinner("Calling Chat endpoint..."):
            if question:
                response = invoke_chat_endpoint(question)
                st.write(response.text)
                st.success("Chat endpoint called successfully.")
            else:
                st.warning("Please enter a question.")

if __name__ == "__main__":
    main()
