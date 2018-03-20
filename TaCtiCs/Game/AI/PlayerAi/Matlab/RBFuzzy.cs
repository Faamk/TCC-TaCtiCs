%ACOR−V % % function y = acorv(population , error_function ,
params_error_function)
% % population is the initial solutions , each solution in a line . Each column % represents a variable of the solution and the last column is the fitness % value of the solution . The maximum number of solutions is k (currently % set in 50). You need to pass at least one solution so the algorithm can % detect the number of variables . % % error_function is the function who receives a solution and return the % equivalent fitness value . Remember to put the @ simbol to pass a handle % to the function . % % params_error_function is a struct with the parameters to the % error_function function [y, it ] = acorv(population , error_function ) global populations global fitnesses global temp populations = zeros(0 , 0) ; fitnesses = zeros(0 , 0) ; fitnesses (: , :) = population (: , end) ; global RBFUZZY k = RBFUZZY.k;
m = RBFUZZY.m; max_value_allowed = 1; min_value_allowed = 0; n = size(population, 2) − 1; max_number_of_iteration = 10000; max_number_of_iteration_without_improvement = ... RBFUZZY.maxNumIteracoesSemMelhora; minimal_size_of_improvement = 10 ^ (−6);
required_error = 0;
i = size(population, 1); while i < k i = i + 1; new_solution = zeros(1, n + 1); for j = 1:n new_solution(j) = min_value_allowed + (rand ∗ (max_value_allowed − min_value_allowed)); end; RBFUZZY. pesos = new_solution (1:n); new_solution(n + 1) = error_function (); population(i, :) = new_solution; end; population = sortrows(population, n + 1);
iterations_without_improvement = 0; size_of_improvement = 0; best_error_up_to_now = population(1, n + 1); error_evolution = zeros(1, max_number_of_iteration); for iteration = 1:max_number_of_iteration new_solutions = zeros(m, n + 1); for ant = 1:m new_solution = zeros(1, n + 1); for j = 1:n new_solution(j) = normrnd(population(1, j), desvpad(population (:, j))); if new_solution(j) < min_value_allowed new_solution(j) = min_value_allowed; elseif new_solution(j) > max_value_allowed new_solution(j) = max_value_allowed; end; end; RBFUZZY. pesos = new_solution (1:n);
populations = [populations; new_solution (1:end −1)]; tmp = error_function (); new_solution(n + 1) = tmp; fitnesses = [fitnesses; tmp]; new_solutions(ant, :) = new_solution;
end; for ant = 1:m population(k + ant, :) = new_solutions(ant, :); end; population = sortrows(population, n + 1); population = population (1:k, :); error_evolution(iteration) = population(1, n + 1); if population(1, n + 1) ~= best_error_up_to_now size_of_improvement = size_of_improvement + best_error_up_to_now − population (1, n + 1); best_error_up_to_now = population(1, n + 1); if size_of_improvement > minimal_size_of_improvement size_of_improvement = 0; iterations_without_improvement = 0; else iterations_without_improvement = iterations_without_improvement + 1; end; else iterations_without_improvement = iterations_without_improvement + 1; if iterations_without_improvement > max_number_of_iteration_without_improvement
break;
end;
end;
if population(1, n + 1) < required_error break; end;
end; it = int32(iteration);
for i = (iteration + 1):max_number_of_iteration error_evolution(i) = 1 / 0; end;
    y = population (1, 1:n);
end
function y = desvpad(x) k = length(x); y = 0; for i = 2:k y = y + (x(i) − x(1)) ^ 2; end; y = sqrt(y / (k−1)) ∗ 0.85; end
function calcularAtivacoes () global RBFUZZY
dados = RBFUZZY. dadosTreino;
RBFUZZY. spreads = []; temp = cell (RBFUZZY.numNeuronios, 1); for i = 1:RBFUZZY.numNeuronios % Pegando o spread dos pontos − spread temp{i} = dados(RBFUZZY. cidx == i , 1:end −1); spread = RBFUZZY. funcaoSpread(dados(RBFUZZY . cidx == i , 1:end−1)) ’; spread(spread == 0) = 1.0e−5; RBFUZZY. spreads = [RBFUZZY. spreads ; spread ’]; end;
ativacoes = zeros(RBFUZZY.numNeuronios, size(dados, 1)); for i = 1:RBFUZZY.numNeuronios centro = RBFUZZY. centros(i, 1:end); spread = RBFUZZY. spreads(i, 1:end); testeX = dados (:, 1:end−1); params = [sqrt(spread); centro]; atvs = zeros(size(testeX ’)); for j = 1: size(testeX, 2) atvs(j, :) = gaussmf(testeX (:, j) ’, params(:, j) ’); end ativacoes(i, :) = min(atvs);
end;
RBFUZZY. ativacoesTreino = ativacoes ’;
ativacoes = zeros(RBFUZZY.numNeuronios, size(RBFUZZY.dadosTest, 1)); for i = 1:RBFUZZY.numNeuronios centro = RBFUZZY. centros(i, 1:end); spread = RBFUZZY. spreads(i, 1:end); testeX = RBFUZZY. dadosTest (:, 1:end−1); params = [sqrt(spread); centro]; atvs = zeros(size(testeX ’)); for j = 1: size(testeX, 2) atvs(j, :) = gaussmf(testeX (:, j) ’, params(:, j) ’); end ativacoes(i, :) = min(atvs); end; RBFUZZY. ativacoesTest = ativacoes ’;
RBFUZZY. alvosTest = RBFUZZY. dadosTest (:, end); RBFUZZY. alvosTreino = RBFUZZY. dadosTreino (:, end);
% dados = RBFUZZY.dadosTreino; % % RBFUZZY. spreads = [];
% for i = 1:RBFUZZY.numNeuronios % % Pegando o spread dos pontos − spread % spread = RBFUZZY.funcaoSpread(dados( RBFUZZY. cidx == i , 1:end−1)) ’; % spread(spread == 0) = 1.0e−5; % RBFUZZY. spreads = [RBFUZZY. spreads ; spread ’]; % end; % % ativacoes = zeros(RBFUZZY.numNeuronios , size ( dados , 1)); % for i = 1:RBFUZZY.numNeuronios % centro = RBFUZZY. centros(i , 1:end); % spread = RBFUZZY. spreads(i , 1:end); % testeX = dados(: , 1:end−1); % % Calculando a gaussiana % pico = mvnpdf(centro , centro , spread) ’; % z = mvnpdf(testeX , centro , spread) ’; % ativacoes(i , :) = z / pico ; % end; % % RBFUZZY. ativacoesTreino = ativacoes ’; % % ativacoes = zeros(RBFUZZY.numNeuronios , size ( RBFUZZY.dadosTest , 1)); % for i=1:RBFUZZY.numNeuronios % centro = RBFUZZY. centros(i , 1:end); % spread = RBFUZZY. spreads(i , 1:end); % testeX = RBFUZZY.dadosTest (: ,1:end−1); % % Calculando a gaussiana . % pico = mvnpdf(centro , centro , spread) ’; % z = mvnpdf(testeX , centro , spread) ’; % ativacoes(i , :) = z / pico ; % end; % RBFUZZY. ativacoesTest = ativacoes ’; % % RBFUZZY. alvosTest = RBFUZZY.dadosTest (: , end) ; % RBFUZZY. alvosTreino = RBFUZZY.dadosTreino (: , end);
end
function clusterizar ()
global RBFUZZY dados = RBFUZZY. dadosTreino (:, 1:end − 1); numNeuronios = RBFUZZY.numNeuronios; [RBFUZZY. cidx, RBFUZZY. centros] = kmeans (... dados, numNeuronios, ’Distance ’, ’ sqeuclidean ’, ’Replicates ’, 20);
RBFUZZY. index = []; for i = 1: RBFUZZY.numNeuronios RBFUZZY. index = [RBFUZZY. index; find(RBFUZZY. cidx == i)]; end
end
function RBFUZZY = configurar(tipo, fonte, varargin) RBFUZZY. porcaoTreino = 1; RBFUZZY.multSpread = 0.8; RBFUZZY.numNeuronios = 26; RBFUZZY.numSaidasFuzzy = 3; RBFUZZY.valorMinNorm = 0; RBFUZZY.valorMaxNorm = 1;
i = 1; while i <= length(varargin)
arg = varargin{i}; if strcmp(arg, ’porcaoTreino ’) i = i + 1; RBFUZZY. porcaoTreino = varargin{i};
elseif strcmp(arg, ’multSpread ’) i = i + 1; RBFUZZY.multSpread = varargin{i};
elseif strcmp(arg, ’numNeuronios ’) i = i + 1;
    RBFUZZY.numNeuronios = varargin{i};
elseif strcmp(arg, ’numSaidasFuzzy ’) i = i + 1; RBFUZZY.numSaidasFuzzy = varargin{i};
elseif strcmp(arg, ’valorMinNorm ’) i = i + 1; RBFUZZY.valorMinNorm = varargin{i};
elseif strcmp(arg, ’valorMaxNorm ’) i = i + 1; RBFUZZY.valorMaxNorm = varargin{i}; end i = i + 1;
end
RBFUZZY.dados = Dados(tipo, fonte, varargin {:});
tamanhoEntrada = RBFUZZY.dados.numeroEntradas();
[dadosNormalizados, RBFUZZY. proporcao] = mapminmax(... RBFUZZY.dados.getDados() ’, ... RBFUZZY.valorMinNorm, RBFUZZY.valorMaxNorm);
dadosNormalizados = dadosNormalizados ’;
[RBFUZZY. indicesTreino, testIndices] = dividerand (... tamanhoEntrada, RBFUZZY. porcaoTreino, 1 − RBFUZZY. porcaoTreino, 0);
RBFUZZY. dadosTreino = dadosNormalizados(RBFUZZY . indicesTreino, :);
if ~ isempty(testIndices) RBFUZZY. dadosTest = dadosNormalizados(
    testIndices, :);
else
    RBFUZZY. dadosTest = dadosNormalizados;
end RBFUZZY. funcaoSpread = @(dados) std(dados, 0, 1) ∗ RBFUZZY.multSpread; end
classdef Dados < handle % A classe DADOS representa os dados de treinamento e os dados de % teste . % Fontes de dados suportadas : % − ’csv ’ , nome do arquivo % − ’dataset ’ , dados disponiveis no matlab % − ’cellArray ’ , celulas (pode ser usado para passar nomes das % variaveis e respectivos dados. % − ’array ’ , array multidimensional properties (Access = private) Tabela; DadosVal; end
    methods function self = Dados(tipo, fonte, varargin) if strcmp(tipo, ’dataset ’) [entradas, saidas] = evalin(’base ’, fonte); self .Tabela = array2table ([entradas ’, saidas ’]); else if strcmp(tipo, ’csv ’) || strcmp(tipo, ’txt ’) self . carregarDadosReais(fonte, varargin {:}); elseif strcmp(tipo, ’cellArray ’)
        self .Tabela = table (... fonte {2:end}, ’ VariableNames ’, fonte {1}); elseif strcmp(tipo, ’array ’) self .Tabela = array2table(fonte); end if any(strcmp(varargin, ’ indiceSaida ’)) pos = find(strcmp(varargin, ’ indiceSaida ’)); indicesSaida = sort(varargin{pos + 1}); saidas = self .Tabela (:, indicesSaida); self .Tabela (:, indicesSaida) = []; self .Tabela = [self .Tabela saidas]; end
      end
end
function carregarDadosReais(self, nomeDoArquivo, varargin) %CARREGADADOSREAIS Importa dados de perfuracao a partir de um %arquivo CSV como uma tabela . % % Os dados devem estar delimitados por ’; ’ e o sepador % decimal deve ser o ’. ’. % % Exemplo: % obj . carregarDadosReais(’<filename >. csv ’) ; % if any(strcmp(varargin , ’ porcentagemTeste ’)) pos = find(strcmp(varargin , ’
porcentagemTeste ’)); self .PorcentagemTeste = varargin{pos + 1};
end
data = importdata(nomeDoArquivo); if isfloat (data) nomes = cell (1, size(data, 2)); for i = 1: size(data, 2) − 1 nomes{i} = strcat (’Input ’, num2str(i)); end nomes{end} = ’Output ’; self .Tabela = array2table(data, ’ VariableNames ’, nomes); else self .Tabela = array2table (... data .data, ’VariableNames ’, data . textdata); end
if any(strcmp(varargin, ’dadosVal ’)) pos = find(strcmp(varargin, ’ dadosVal ’)); self .DadosVal = varargin{pos + 1}; end
end
function nomesColunas = nomesColunas(self) cells = table2cell (self .Tabela (1, :)); colunasNumericas = cellfun (@isnumeric, cells); colunas = properties (self .Tabela); nomesColunas = colunas(colunasNumericas (:), 1); end
function qEntradas = numeroEntradas(self) qEntradas = size(self .Tabela, 1); end
function dadosVal = getDadosVal(self) if ~ isempty(self .DadosVal) dadosVal = self .DadosVal; else dadosVal = table2array(self .Tabela); end end
function dados = getDados(self) dados = table2array(self .Tabela); end
function dados = getIndices (self, indices) dados = table2array(self .Tabela); dados = dados(indices, :); end
end
end
function fit = fitness () global RBFUZZY temp = simular(RBFUZZY. ativacoesTreino); fit = mse(RBFUZZY. alvosTreino − temp); end
function fis = gerarFis(salvarEmArquivo) global RBFUZZY
pesos = reshape (... RBFUZZY. pesos, RBFUZZY.numSaidasFuzzy, RBFUZZY.numNeuronios) ’; numEntradas = size(RBFUZZY. dadosTreino, 2) − 1; centros = RBFUZZY. centros; spreads = sqrt(RBFUZZY. spreads); nomesVariaveis = RBFUZZY.dados.nomesColunas();
  if salvarEmArquivo [arquivo, caminho] = uiputfile(’∗. fis ’, ’ Salvar␣FIS ’); if isequal (arquivo, 0) || isequal (caminho, 0)
    arquivo = [’autofis_ ’, strrep(date, ’−’, ’_’), ’ . fis ’]; caminho = ’ ’; uiwait(warndlg ([’Salvando␣como␣ ’, arquivo], ’Cancelado ’, ’modal ’));
end
else
    caminho = tempdir; arquivo = [’autofis_ ’, strrep(date, ’−’, ’_ ’), ’ . fis ’];
end
fis = fopen([caminho, arquivo], ’wt’);
conjuntos_fuzzy = cell (numEntradas, 1); map_conjuntos = cell (size(centros, 1), numEntradas + 1); if RBFUZZY. withCutOff for i = 1:numEntradas conjuntos = [centros (:, i) spreads (:, i)]; similares = clusterdata (conjuntos, RBFUZZY. cutOff); conjuntos_reduzidos = NaN(size(centros, 1), 2); contador = 1; passo = 1; while contador <= max(similares) indices = find(similares (passo) == similares); passo = passo + 1; if isnan(conjuntos_reduzidos(indices (1))) map_conjuntos(indices, i) = {indices ’}; conjunto = conjuntos(indices, :); if size(conjunto, 1) > 1 conjunto = mean(conjunto); end contador = contador + 1;
conjuntos_reduzidos(indices, :) = ... repmat(conjunto, size(indices, 1), 1);
  end
end conjuntos_fuzzy(i) = {conjuntos_reduzidos};
end
else
    for i = 1: size(map_conjuntos, 1) map_conjuntos(i, :) = {i}; end for i = 1:numEntradas conjuntos_fuzzy(i) = {[centros (:, i) spreads (:, i)]}; end
    end
    verificados = false (1, size(map_conjuntos, 1)); for i = 1: size(map_conjuntos, 1) if ~ verificados (i) verificados (i) = true; antecedente = map_conjuntos{i, 1}; consequente = map_conjuntos{i, 2}; s = length(antecedente) == length(consequente); if length(antecedente) > 1 && s && all (antecedente == consequente) j = i + 1; while j <= size(map_conjuntos, 1) s = length(antecedente) == length(map_conjuntos{j, 1}); if s && all (antecedente == map_conjuntos{j, 1}) map_conjuntos{i, end} = max (pesos(i, :), pesos(j, :)); map_conjuntos{j, end} = −
    ones(1, RBFUZZY. numSaidasFuzzy); verificados (j) = true;
end j = j + 1;
end
else
    map_conjuntos{i, end} = pesos(i, :);
end
end if i == size(map_conjuntos, 1) && ~ verificados (i) map_conjuntos{i, end} = pesos(i, :); end
end regras = NaN(size(pesos, 1) ∗ 3, numEntradas + 2); for i = 1: size(map_conjuntos, 1) conjuntos = map_conjuntos(i, 1:end−1); pesosRegra = map_conjuntos{i, end}; mfs = zeros(1, size(conjuntos, 2)); for j = 1: size(conjuntos, 2) mfs(j) = conjuntos{j}(1); end for j = 1: size(pesosRegra, 2) regras ((i − 1) ∗ 3 + j, :) = [mfs j pesosRegra(j)]; end end
fprintf(fis, ’ %s\n’ , ’ [System] ’) ; partes = strsplit (arquivo , ’ . ’) ; fprintf( fis , ’%s\n’ , [ ’Name=’ ’ ’ , partes{1}, ’ ’ ’ ’ ]) ; fprintf( fis , ’%s\n’ , ’Type=’ ’mamdani’ ’ ’) ; fprintf( fis , ’%s\n’ , ’Version=2.0 ’) ; fprintf( fis , ’%s\n’ , [ ’NumInputs=’ , int2str( numEntradas) ]) ; fprintf( fis , ’%s\n’ , ’NumOutputs=1’) ;
fprintf(fis, ’ %s\n’ , [ ’NumRules=’ , int2str (... sum(regras (: , end) >= RBFUZZY. fatorDeCorte) ) ]) ; fprintf( fis , ’%s\n’ , ’AndMethod=’ ’min’ ’ ’) ; fprintf( fis , ’%s\n’ , ’OrMethod=’ ’max’ ’ ’) ; fprintf( fis , ’%s\n’ , ’ImpMethod=’ ’min’ ’ ’) ; fprintf( fis , ’%s\n’ , ’AggMethod=’ ’max’ ’ ’) ; fprintf( fis , ’%s\n\n’ , ’DefuzzMethod=’ ’centroid ’ ’ ’) ;
mapeamentoMFs = cell (1, 2); for i = 1:numEntradas conjuntos = conjuntos_fuzzy{i}; fprintf(fis, ’ %s\n’ , [ ’ [ Input ’ , int2str( i ) , ’ ] ’ ]) ; fprintf( fis , ’%s\n’ , [ ’Name=’ ’ ’ , nomesVariaveis{i }, ’ ’ ’ ’ ]) ; fprintf( fis , ’%s\n’ , ’Range=[0␣1] ’) ; if RBFUZZY. withCutOff fprintf( fis , ’%s\n’ , [ ’NumMFs=’ , ... int2str(size(unique(conjuntos , ’ rows ’) , 1)) ]) ; else fprintf( fis , ’%s\n’ , [ ’NumMFs=’ , ... int2str(size(conjuntos (: , 1) , 1)) ]) ; end if RBFUZZY. withCutOff [ conjuntos , mapeamento, ~] = unique( conjuntos , ’rows ’ , ’ stable ’) ; for j = 1: size(conjuntos , 1) fprintf( fis , ’%s\n’ , [ ’MF’ , int2str ( j ) , ’=’ ’FP’ , ... int2str( j ) , ’ ’ ’ : ’ ’gaussmf ’ ’ ,[ ’ , ... num2str(conjuntos(j , 2) , 4) , ... ’␣ ’ , num2str(conjuntos(j , 1) , 4) , ’ ] ’ ]) ;
end
mapTo = 1: size(conjuntos, 1); mapeamentoMFs(i) = {[mapeamento mapTo ’]};
else
    for j = 1: size(conjuntos, 1) fprintf(fis, ’ %s\n’ , [ ’MF’ , int2str ( j ) , ’=’ ’FP’ , ... int2str( j ) , ’ ’ ’ : ’ ’gaussmf ’ ’ ,[ ’ , ... num2str(conjuntos(j , 2) , 4) , ... ’␣ ’ , num2str(conjuntos(j , 1) , 4) , ’ ] ’ ]) ;
    end mapTo = 1: size(conjuntos, 1); mapeamentoMFs(i) = {[mapTo’ mapTo’]};
end fprintf(fis, ’ \ n’);
end
fprintf(fis, ’ %s\n’ , strcat ( ’ [Output1] ’)) ; fprintf( fis , ’%s\n’ , [ ’Name=’ ’ ’ , nomesVariaveis {end}, ’ ’ ’ ’ ]) ; fprintf( fis , ’%s\n’ , ’Range=[−0.5␣1.5] ’) ; fprintf( fis , ’%s\n’ , ’NumMFs=3’) ; fprintf( fis , ’%s\n’ , ’MF1=’ ’Baixa ’ ’ : ’ ’trimf ’ ’ ,[−0.5␣0␣0.5] ’) ;fprintf ( fis , ’%s\n’ , ’MF2=’ ’Media ’ ’ : ’ ’trimf ’ ’ ,[0␣0.5␣1] ’) ; fprintf( fis , ’%s\n\n’ , ’MF3=’ ’Alta ’ ’ : ’ ’trimf ’ ’ ,[0.5␣1␣1.5] ’) ;
fprintf(fis, ’ %s\n’ , ’ [ Rules ] ’) ;
for i = 1: size(regras, 1) regra = regras(i, :); mfs = zeros(1, numEntradas); for j = 1: size(mfs, 2) origem = mapeamentoMFs{j}(:, 1); destino = mapeamentoMFs{j}(:, 2);
    mfs(1, j) = destino(regra(j) == origem);
end peso = regra(end); if peso < RBFUZZY. fatorDeCorte continue; else fprintf(fis, ’ %s\n’ , [ int2str(mfs) , ’ ,␣ ’ , ... int2str(regra(end−1)) , ’␣( ’ , num2str( peso) , ’) ’ , ’␣:␣1 ’ ]) ; end
end
fclose(fis); fis = readfis ([caminho, arquivo]);
end
function [saida] = inferenciaRBF(dados) global RBFUZZY ativacoes = zeros(RBFUZZY.numNeuronios, size(dados, 1)); for i = 1:RBFUZZY.numNeuronios centro = RBFUZZY. centros(i, 1:end); spread = RBFUZZY. spreads(i, 1:end); params = [sqrt(spread); centro]; atvs = zeros(size(dados ’)); for j = 1: size(dados, 2) atvs(j, :) = gaussmf(dados (:, j) ’, params(:, j) ’); end ativacoes(i, :) = min(atvs); end; saida = simular(ativacoes ’); end
function y = inferrenciaFuzzy(ativacoes, pesos, numSaidasFuzzy, intervaloNorm) valorMin = intervaloNorm(1) − 0.5; valorMax = intervaloNorm(2) + 0.5;
passo = (valorMax − valorMin) / (numSaidasFuzzy + 1); intervalo = valorMin : passo :valorMax; % passoX = passo / 2;
% x = valorMin:passoX:valorMax; x = linspace(valorMin , valorMax , 101) ; % mf = zeros(numSaidasFuzzy , numSaidasFuzzy ∗ numSaidasFuzzy); mf = zeros(numSaidasFuzzy , size(x, 2)) ; for i = 1: numSaidasFuzzy
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% trimf %%%%%%%%%%%%%%%%%%%%%%%%%%%%% a = intervalo ( i ) ; b = intervalo ( i + 1) ; c = intervalo ( i + 2) ;
y = zeros(size(x));
indices = find(x <= a | c <= x); y(indices) = zeros(size(indices)); if (a ~= b) indices = find(a < x & x < b); y(indices) = (x(indices) − a) / (b − a); end if (b ~= c) indices = find(b < x & x < c); y(indices) = (c − x(indices)) / (c − b); end indices = find(x == b); y(indices) = ones(size(indices)); mf(i, :) = y;
%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
end
W = reshape(pesos, numSaidasFuzzy, length(pesos) / numSaidasFuzzy) ’; % mft = zeros(numSaidasFuzzy , numSaidasFuzzy ∗ numSaidasFuzzy); mft = zeros(numSaidasFuzzy , size(x, 2)) ; for j = 1:numSaidasFuzzy mft(j , :) = min(ativacoes (1) ∗ W(1 , j ) , mf(j , :) ) ; end
for i = 2:length(ativacoes) for j = 1:numSaidasFuzzy mft(j, :) = max(min(ativacoes(i) ∗ W(i, j), mf(j, :)), mft(j, :)); end end
    %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% defuzz %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% try %y = defuzz(x, max(mft) , ’centroid ’) ; mf = max(mft) ; mf = mf(:) ; y = sum(mf.∗x’)/sum(mf) ;catch y = 1000; end % %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
end
function otimizar ()
global RBFUZZY global temp temp = zeros(450, 1); datestr(now, ’HH:MM:SS’);
numPesos = RBFUZZY.numNeuronios ∗ 3; populacao = rand(RBFUZZY.k, numPesos + 1); ativacoes = RBFUZZY. ativacoesTreino; alvos = RBFUZZY. alvosTreino; intervaloNorm = [RBFUZZY.valorMinNorm RBFUZZY. valorMaxNorm];
counter = 1; atvSize = size(ativacoes, 1); for j = 1:RBFUZZY.k erros = []; p = populacao(j, 1:numPesos) ’; for i = 1: atvSize a = ativacoes(i, :) ’; y = inferrenciaFuzzy(a, p, RBFUZZY. numSaidasFuzzy, intervaloNorm); temp(counter) = alvos(i) − y; counter = counter + 1; erros = [erros, alvos(i) − y]; end fit = mse(erros); populacao(j, numPesos + 1) = fit; end
[RBFUZZY. pesos, it] = acorv(populacao, @fitness); if (it − RBFUZZY.maxNumIteracoesSemMelhora) == 1 otimizar (); end
datestr(now, ’HH:MM:SS’);
end
dbstop if error
global RBFUZZY global populations global fitnesses
RBFUZZY = configurar (... ’csv ’, dadosArquivo, ... ’porcaoTreino ’, 1, ... ’multSpread ’, 1, ... ’numNeuronios ’, 7);
RBFUZZY.k = 50; RBFUZZY.m = 2; RBFUZZY.maxNumIteracoesSemMelhora = 100;
% Fator de corte utilizado para eliminar gaussianas % Veja clusterirar .m RBFUZZY. cutOff = 0.75; RBFUZZY. withCutOff = false ;
% Fator de corte no peso. RBFUZZY. fatorDeCorte = 0;
RBFUZZY. teste = [];
clusterizar (); calcularAtivacoes (); otimizar ();
FIS = gerarFis(false);
function saidaSimulada = simular(ativacoes) global RBFUZZY intervaloNorm = [RBFUZZY.valorMinNorm RBFUZZY. valorMaxNorm]; saidaSimulada = zeros(size(ativacoes, 1), 1); for j = 1: size(ativacoes, 1) saidaSimulada(j, 1) = inferrenciaFuzzy(ativacoes(j, :) ’, ... RBFUZZY. pesos, RBFUZZY.numSaidasFuzzy, intervaloNorm); end end
function virgulaParaPonto(nomeDoArquivo)
%VIRGULAPARAPONTO converte de virgula para ponto em numeros decimais .
texto = fileread (nomeDoArquivo); texto = strrep(texto, ’, ’, ’ . ’); arquivo = fopen(nomeDoArquivo, ’w’); fprintf(arquivo, ’ %s ’ ,texto) ; fclose(arquivo) ;
end
