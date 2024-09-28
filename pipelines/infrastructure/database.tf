resource "azurerm_mssql_server" "sohat_db_server" {
  name                         = "sohatdbserver"
  resource_group_name          = azurerm_resource_group.az_rg.name
  location                     = azurerm_resource_group.az_rg.location
  administrator_login          = var.db_username
  administrator_login_password = var.db_password
  version                      = "12.0"

  tags = {
    environment = "development"
  }
}

resource "azurerm_mssql_database" "sohat_db" {
  name      = "sohatdb"
  server_id = azurerm_mssql_server.sohat_db_server.id
  sku_name  = "Basic"

  tags = {
    environment = "development"
  }
}