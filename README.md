<h2> 
  Sobre o projeto
</h2>

<p> 
  Esse projeto foi desenvolvido baseado em um teste técnico. 
</p> 

<p> 
  Você escolhe uma pasta com vários CSV com nome no seguinte formato: 
  </br>
  "Nome do Departamento - Mês Vigência - Ano Vigência"
</p>

<p>
  Exemplo:
  </br>
  Departamento1-Abril-2022.csv
</p>

<p> 
  Contendo informações de funcionários separadas por ' ; ' ordenados da seguinte forma: 
  </br>
  Código;NomeFuncionário;ValorHora(R$ 00,00);Data(dd-MM-yyyy);HoraEntrada(HH:mm:ss);HoraSaída(HH:mm:ss);Almoço(00:00-00:00)
</p>

<p>
  Exemplo: 
  </br>
  01;Funcionário um; R$ 15,00;10-10-2022;08:00:00;17:00:00;12:00 - 13:00;
  02;Funcionário dois; R$ 14,50;10-10-2022;09:00:00;17:00:00;12:00 - 13:00;
  01;Funcionário um; R$ 15,00;10-10-2022;08:00:00;18:00:00;12:00 - 13:00;
</p>

<p>
  O output é um json que detalha informações do departamento e de todos funcionários, horas extras feitas por funcionários, horas faltantes, pagamento extra, pagamento total e descontos.
</p>
