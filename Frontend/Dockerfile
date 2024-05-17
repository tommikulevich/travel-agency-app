FROM node:18-alpine

WORKDIR /app
EXPOSE 3000

ENV PATH /app/node_modules/.bin:$PATH

COPY ./Frontend/package.json ./
COPY ./Frontend/package-lock.json ./

RUN npm install 

COPY ./Frontend ./

CMD ["npm", "start"]