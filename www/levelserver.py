import os
from flask import Flask, request, session, g, redirect, url_for, abort, render_template, flash

# make an application
app = Flask(__name__)
app.config.from_object(__name__)

# load default config
app.config.update( dict(
    DEBUG=True
))
# override any of the defaults with stuff from an env variable
#app.config.from_envvar('LEVELLOADER_SETTINGS', silent=True)

# the routes
@app.route('/level/')
def show_levels():
    """get a list of the available levels"""
    return "one\ntwo\nthree"

@app.route('/level/<level_name>')
def get_level( level_name):
    """get a specified level"""
    return """# loaded from URL
1512222222
1011140011
1011101101
1011101101
1011101101
1011101101
1011100011
1011112111
1000000111
1111111111"""

if __name__ == '__main__':
    app.run()
