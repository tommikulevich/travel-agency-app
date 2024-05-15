FROM node:18-alpine

WORKDIR /app
EXPOSE 3000

ENV PATH /app/node_modules/.bin:$PATH

COPY ./rsww-front/package.json ./
COPY ./rsww-front/package-lock.json ./

RUN npm install --omit=dev 

COPY ./rsww-front ./

CMD ["npm", "start"]